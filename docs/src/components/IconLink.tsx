import Link from 'next/link'
import clsx from 'clsx'

export function IconLink({
  children,
  className,
  compact = false,
  large = false,
  followTheme,
  icon: Icon,
  ...props
}: React.ComponentPropsWithoutRef<typeof Link> & {
  compact?: boolean
  large?: boolean
  icon?: React.ComponentType<{ className?: string }>,
  followTheme?: boolean
}) {
  return (
    <Link
      {...props}
      className={clsx(
        className,
        followTheme ? 'dark:text-white/30 text-black/30' : 'text-white/30',
        'group relative isolate flex items-center rounded-lg px-2 py-0.5 text-[0.8125rem]/6 font-medium transition-colors hover:text-sky-600 dark:hover:text-sky-300',
        compact ? 'gap-x-2' : 'gap-x-3',
      )}
    >
      <span className={clsx(followTheme ? 'bg-black/5 dark:bg-white/5' : 'bg-white/5', 'absolute inset-0 -z-10 scale-75 rounded-lg opacity-0 transition group-hover:scale-100 group-hover:opacity-100')} />
      {Icon && (
        <Icon className={clsx('flex-none', large ? 'h-6 w-6' : 'h-4 w-4')} />
      )}
      <span className={clsx(followTheme ? 'dark:text-white text-black' : 'text-white', 'self-baseline')}>{children}</span>
    </Link>
  )
}
