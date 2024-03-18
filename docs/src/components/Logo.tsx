import { useId } from 'react'

export function Logo(props: React.ComponentPropsWithoutRef<'svg'>) {
  let id = useId()

  return (
    <svg viewBox="0 0 44.358 44.358" fill="none" aria-hidden="true" {...props}>
      <title>orels Layout Toolkit</title>
      <path
        fillRule="evenodd"
        clipRule="evenodd"
        d="m29.557,8.438V1.262C26.819.296,23.838-.154,20.726.047,9.539.768.539,9.934.024,21.133c-.601,13.076,10.126,23.802,23.202,23.201,11.199-.515,20.365-9.515,21.086-20.702.2-3.112-.249-6.093-1.215-8.83h-7.176c-3.515,0-6.364-2.849-6.364-6.364Zm-7.378,23.121c-5.18,0-9.379-4.199-9.379-9.379s4.199-9.379,9.379-9.379,9.379,4.199,9.379,9.379-4.199,9.379-9.379,9.379Z"
        fill={`url(#${id}-g)`}
      />
      <defs>
        <linearGradient
          id={`${id}-g`}
          x1={2.88}
          y1={0}
          x2={35.88}
          y2={30}
          gradientUnits="userSpaceOnUse"
        >
          <stop stopColor="#bef264" />
          <stop offset={1} stopColor="#38bdf8" />
        </linearGradient>
      </defs>
    </svg>
  )
}
