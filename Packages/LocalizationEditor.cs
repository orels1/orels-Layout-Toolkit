using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ORL.Localization.Attributes;
using ORL.Localization.Elements;
using ORL.Localization.Extensions;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ORL.Localization
{
    public class LocalizationEditor : EnhancedEditorWindow
    {
        [MenuItem("orels1/Localization")]
        private static void ShowWindow()
        {
            var window = GetWindow<LocalizationEditor>();
            window.titleContent = new GUIContent("Localization Editor");
            window.Show();
        }

        [MenuItem("orels1/Kill Localization")]
        private static void KillWindow()
        {
            var window = GetWindow<LocalizationEditor>();
            if (window != null)
            {
                try
                {
                    window.Close();
                }
                finally
                {
                    DestroyImmediate(window);
                }
                
            }
        }
        
        [BoundProperty]
        private ReactiveProperty<string> Languages { get; } = new ReactiveProperty<string>();

        public LocalizationDB db;

        private VisualElement _r;
        
        private void CreateGUI()
        {
            _r = rootVisualElement;
            Resources.Load<VisualTreeAsset>("LocalizationEditorLayout").CloneTree(_r);
            _r.styleSheets.Add(Resources.Load<StyleSheet>("LocalizationEditorUtilityStyles"));
            _r.styleSheets.Add(Resources.Load<StyleSheet>("LocalizationEditorStyles"));
            var sO = new SerializedObject(this);
            _r.Add(VStack(
                Button(ReloadGUI).WithText("Reload UI"),
                HStack(
                    PropertyField(sO.FindProperty("db")).Flex(1),
                    Button(SetFirstDB).WithText("Select first Database")
                ),
                HStack(
                    Label("Supported Languages:"),
                    Label().BindToProp(Languages)
                ).Margin(2, 2, 4, 2),
                Foldout().WithText("Strings").WithClass("unity-foldout").WithViewDataKey("strings-foldout")
                    .WithChildren(
                        ScrollView().WithName("stringsContainer"),
                        Button(AddString).WithText("Add New String")
                    )
            ));
            _r.Bind(sO);
            

            var methods = GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var method in methods)
            {
                var attributes = method.GetCustomAttributes();
                foreach (var attribute in attributes)
                {
                    if (attribute is BoundButton button)
                    {
                        var found = _r.Q<Button>(button.Name);
                        if (found == null) continue;
                        found.clickable.clicked += () => method.Invoke(this, null);
                    }
                }
            }

            var props = GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var prop in props)
            {
                var attributes = prop.GetCustomAttributes();
                foreach (var attribute in attributes)
                {
                    if (attribute is BoundProperty boundProp)
                    {
                        if (prop.GetValue(this, null) == null)
                        {
                            prop.SetValue(this, Activator.CreateInstance(prop.PropertyType));
                        }
                        if (string.IsNullOrWhiteSpace(boundProp.Name)) continue;
                        var rP = prop.GetValue(this, null);
                        var changeEvent = rP.GetType().GetEvent("OnValueChanged");
                        
                        if (boundProp.TargetType == typeof(Label))
                        {
                            var found = _r.Q<Label>(boundProp.Name);
                            changeEvent.AddEventHandler(rP, new Action<string>(s => found.text = s));
                        } else if (boundProp.TargetType == typeof(VisualElement))
                        {
                            var found = _r.Q(boundProp.Name);
                            changeEvent.AddEventHandler(rP, new Action<VisualElement>(el =>
                            {
                                found.Clear();
                                found.Add(el);
                            }));
                        }
                    }
                }
            }
        }
        
        private void ReloadGUI()
        {
            _r.Clear();
            CreateGUI();
        }
        
        private void SetFirstDB()
        {
            var allDbs = AssetDatabase.FindAssets($"t: {typeof(LocalizationDB).Name}").ToList()
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<LocalizationDB>)
                .ToList();
            if (allDbs.Count > 0)
            {
                db = allDbs[0];
            }

            if (db.languages == null || db.languages.Count == 0)
            {
                Languages.Set("None");
            }
            else
            {
                Languages.Set(string.Join(", ", db.languages));
            }
            RenderStrings();
        }
        
        private void RenderStrings()
        {
            if (db == null) return;
            if (db.strings == null) return;
            if (db.strings.Count == 0) return;

            var container = _r.Q("stringsContainer");
            container.Clear();
            container.Add(VStack(
                HStack(
                    Label("String Id").Flex(1).Bold().Padding(0, 4),
                    ForEach(db.languages, s => Label(s).Flex(1).Bold().Padding(0, 4)),
                    Label().Width(100)
                ),
                ForEach(db.strings, (dbString, stringIndex) =>
                    HStack(
                        new TextField()
                            .Flex(1)
                            .WithValue(dbString.Id)
                            .WithUserData(stringIndex)
                            .IsDelayed()
                            .OnChange(UpdateStringId),
                        ForEach(dbString.Values.Values, (dbStringValue, index) => new TextField()
                            .IsDelayed()
                            .Flex(1)
                            .WithValue(dbStringValue)
                            .WithUserData((stringIndex, dbString.Values.Keys.ToList()[index]))
                            .OnChange(UpdateStringValue)
                        ),
                        Button(RemoveString(stringIndex)).WithText("Remove").Width(100)
                    )
                )
            ));
        }

        private void UpdateStringId(ChangeEvent<string> e)
        {
            var index = (int) ((VisualElement) e.target).userData;
            var changedString = db.strings[index];
            changedString.Id = e.newValue;
            Undo.RecordObject(db, "Edited String Id");
            db.strings[index] = changedString;
            Save();
        }
        
        private void UpdateStringValue(ChangeEvent<string> e)
        {
            var data = ((int stringIndex, string languageKey)) ((VisualElement) e.target).userData;
            var changedString = db.strings[data.stringIndex];
            changedString.Values[data.languageKey] = e.newValue;
            Undo.RecordObject(db, "Edited String Value");
            db.strings[data.stringIndex] = changedString;
            Save();
        }


        private void AddLanguage(string language)
        {
            if (db == null) return;
            Undo.RecordObject(db, "Add Language");
            db.languages.Add(language);
            if (db.strings.Count > 0)
            {
                foreach (var localizedString in db.strings)
                {
                    localizedString.Values.Add(language, string.Empty);
                }
            }
            
            if (db.materials.Count > 0)
            {
                foreach (var localizedMaterial in db.materials)
                {
                    localizedMaterial.Values.Add(language, null);
                }
            }
            
            if (db.textures.Count > 0)
            {
                foreach (var localizedTexture in db.textures)
                {
                    localizedTexture.Values.Add(language, null);
                }
            }
            
            if (db.audioClips.Count > 0)
            {
                foreach (var localizedAudioClip in db.audioClips)
                {
                    localizedAudioClip.Values.Add(language, null);
                }
            }
            Save();
            
            RenderStrings();
        }

        private void RemoveLanguage(string language)
        {
            Undo.RecordObject(db, "Remove Language");
            db.languages.Remove(language);
            if (db.strings.Count > 0)
            {
                foreach (var localizedString in db.strings)
                {
                    localizedString.Values.Remove(language);
                }
            }
            
            if (db.materials.Count > 0)
            {
                foreach (var localizedMaterial in db.materials)
                {
                    localizedMaterial.Values.Remove(language);
                }
            }
            
            if (db.textures.Count > 0)
            {
                foreach (var localizedTexture in db.textures)
                {
                    localizedTexture.Values.Remove(language);
                }
            }
            
            if (db.audioClips.Count > 0)
            {
                foreach (var localizedAudioClip in db.audioClips)
                {
                    localizedAudioClip.Values.Remove(language);
                }
            }
            Save();
            RenderStrings();
        }

        private void AddString()
        {
            if (db == null) return;
            var newString = new LocalizationDB.LocalizedString
            {
                Id = "LocalizedString",
                Values = new Dictionary<string, string>()
            };
            foreach (var dbLanguage in db.languages)
            {
                newString.Values.Add(dbLanguage, string.Empty);
            }
            Undo.RecordObject(db, "Added String");
            db.strings.Add(newString);
            Save();
            RenderStrings();
        }

        private Action RemoveString(int index)
        {
            return () =>
            {
                if (db == null) return;
                Undo.RecordObject(db, "Added String");
                db.strings.RemoveAt(index);
                Save();
                RenderStrings();
            };
        }

        private void Save()
        {
            EditorUtility.SetDirty(db);
            AssetDatabase.SaveAssets();
        }
    }
}