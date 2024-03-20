import { useId } from 'react';
import { ThemeToggle } from '@/components/ThemeToggle';
import { Logo } from './Logo';
import { BookIcon, GitHubIcon, ListIcon, TwitterIcon, MastodonIcon, DiscordIcon } from './GeneralIcons';
import { IconLink } from './IconLink';
import Link from 'next/link';


export function GlowHorizontal() {
  let id = useId()

  return (
    <div className="absolute inset-0 -z-10 overflow-hidden bg-gray-950">
      <svg
        className="absolute -bottom-48 left-[-40%] h-[80rem] w-[180%]"
        aria-hidden="true"
      >
        <defs>
          <radialGradient id={id} cy="100%">
            <stop offset="0%" stopColor="rgba(22, 74, 97, 0.3)" />
            <stop offset="53.95%" stopColor="rgba(23, 51, 96, 0.09)" />
            <stop offset="100%" stopColor="rgba(1, 41, 63, 0)" />
          </radialGradient>
        </defs>
        <rect
          width="100%"
          height="100%"
          fill={`url(#${id})`}
        />
      </svg>
      <div className="absolute inset-x-0 bottom-0 right-0 h-px bg-white mix-blend-overlay" />
    </div>
  )
}

const MENU_ITEMS: Record<string, Array<{
  name: string;
  href: string;
}>> = {
  'Get Started': [
    { name: 'Installation', href: '#installation' },
    { name: 'Basic Setup', href: '#basic-setup' },
  ],
  'Elements': [
    { name: 'HStack', href: '#hstack' },
    { name: 'VStack', href: '#vstack' },
    { name: 'ForEach', href: '#foreach' },
  ],
  'Utility Methods': [
    { name: 'Style', href: '#style' },
    { name: 'Class', href: '#class' },
    { name: 'ViewDataKey', href: '#viewdatakey' },
    { name: 'ShowIf', href: '#showif' },
    { name: 'HideIf', href: '#hideif' },
    { name: 'BindVisibleState', href: '#bindvisiblestate' },
    { name: 'BoundPropText', href: '#boundproptext' },
    { name: 'BoundPropValue', href: '#boundpropvalue' }
  ],
  'Escape Hatches': [
    { name: 'Overview', href: '#escape-hatches' },
  ],
  'API Reference': [
    { name: 'All Methods', href: '#api-reference' },
  ],
}

export function DocsLayout({ children }: { children: React.ReactNode }) {
  return (
    <>
      <div className="flex relative w-full py-4 mb-8">
        <div className="container m-auto flex flex-col lg:flex-row items-center justify-between">
          <div className="flex">
            <Link href="/" className="inline-flex items-center gap-3">
              <Logo className="inline-block h-11 w-auto" />
              <span className="inline-block text-2xl font-semibold text-white font-display">orels Layout Toolkit</span>
            </Link>
          </div>
          <div className="flex items-center gap-3">
            <IconLink href="/docs" icon={BookIcon} className="flex-none">
              Docs
            </IconLink>
            <IconLink href="/#changelog" icon={ListIcon} className="flex-none">
              Changelog
            </IconLink>
            <IconLink href="https://github.com/orels1/orels-Layout-Toolkit" icon={GitHubIcon} className="flex-none">
              GitHub
            </IconLink>
          </div>
        </div>
        <GlowHorizontal />
      </div>
      <ThemeToggle alwaysDark />
      <div className="relative flex-auto flex">
        <main className="container mx-auto flex flex-col lg:flex-row pb-8 gap-4 px-2 lg:px-0">
          <div className="flex-col lg:basis-1/5 overflow-y-auto h-full hidden lg:flex">
            <ul className="list-inside space-y-6">
              {Object.entries(MENU_ITEMS).map(([key, elements]) => (
                <li key={key}>
                  <span className="font-bold text-lg inline-block mb-3">{key}</span>
                  <ul className="list-inside space-y-3">
                    {elements.map(el => (
                      <li key={el.name} className="opacity-70 hover:opacity-100 transition-opacity">
                        <span className="inline-block mr-4">•</span>
                        <Link href={el.href}>{el.name}</Link>
                      </li>
                    ))}
                  </ul>
                </li>
              ))}
            </ul>
          </div>
          <div className="flex flex-col lg:basis-3/5 typography">
            {children}
          </div>
          <div className="flex-col lg:basis-1/5 lg:h-full">
            <div className="font-bold text-lg inline-block mb-3">Resources</div>
            <div className="flex flex-col gap-2">
              <IconLink href="https://discord.gg/orels1" icon={DiscordIcon} followTheme compact large>
                Support Discord
              </IconLink>
              <IconLink href="https://twitter.com/orels1_" icon={TwitterIcon} followTheme compact large>
                orels1_
              </IconLink>
              <IconLink href="https://mastodon.gamedev.place/@orels1" icon={MastodonIcon} followTheme compact large>
                orels1
              </IconLink>
            </div>
          </div>
        </main>
      </div>
    </>
  )
}