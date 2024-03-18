import { type Metadata } from 'next'
import { Inter } from 'next/font/google'
import localFont from 'next/font/local'
import clsx from 'clsx'

import { Providers } from '@/app/providers'

import '@/styles/tailwind.css'

const inter = Inter({
  subsets: ['latin'],
  display: 'swap',
  variable: '--font-inter',
})

const monaSans = localFont({
  src: '../fonts/Mona-Sans.var.woff2',
  display: 'swap',
  variable: '--font-mona-sans',
  weight: '200 900',
})

export const metadata: Metadata = {
  title: 'orels Layout Toolkit - Build UIToolkit Editor UIs faster with a Fluent interface inspired by SwiftUI',
  description:
    'orels Layout Toolkit is a simple layout engine which allows you to build complex UIs with ease. It is inspired by SwiftUI and uses the Fluent API pattern to quickly scaffold complex and reactive UI hierarchies.',
  alternates: {
    types: {
      'application/rss+xml': `${process.env.NEXT_PUBLIC_SITE_URL}/feed.xml`,
    },
  },
  metadataBase: new URL('https://layout.orels.sh'),
  openGraph: {
    type: 'website',
    locale: 'en_US',
    url: 'https://layout.orels.sh',
    description: 'orels Layout Toolkit is a simple layout engine which allows you to build complex UIs with ease. It is inspired by SwiftUI and uses the Fluent API pattern to quickly scaffold complex and reactive UI hierarchies.',
    title: 'orels Layout Toolkit',
    siteName: 'orels Layout Toolkit',
    images: '/splash.png',
  },
  twitter: {
    site: '@orels1_',
    card: 'summary_large_image',
    description: 'orels Layout Toolkit is a simple layout engine which allows you to build complex UIs with ease. It is inspired by SwiftUI and uses the Fluent API pattern to quickly scaffold complex and reactive UI hierarchies.',
    title: 'orels Layout Toolkit',
    images: '/splash.png',
  },
}
export default function RootLayout({
  children,
}: {
  children: React.ReactNode
}) {
  return (
    <html
      lang="en"
      className={clsx('h-full antialiased', inter.variable, monaSans.variable)}
      suppressHydrationWarning
    >
      <body className="flex min-h-full flex-col bg-white dark:bg-gray-950">
        <Providers>{children}</Providers>
      </body>
    </html>
  )
}
