import clsx from "clsx";

export default function AddIcon({
  className
}: {
  className?: string;
}) {
  return (
    <svg className={clsx('w-6 h-6', className)} xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor">
      <path strokeLinecap="round" strokeLinejoin="round" d="M12 9v6m3-3H9m12 0a9 9 0 11-18 0 9 9 0 0118 0z" />
    </svg>
  )
}