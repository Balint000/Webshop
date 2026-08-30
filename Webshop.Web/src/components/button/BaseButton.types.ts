import type { ButtonHTMLAttributes } from 'vue'

export type BaseButtonVariant =
  | 'primary'
  | 'secondary'
  | 'success'
  | 'danger'
  | 'warning'
  | 'glass'
  | 'gradient'
  | 'outline'
  | 'ghost'

export type BaseButtonColor =
  | 'primary'
  | 'secondary'
  | 'success'
  | 'warning'
  | 'error'
  | 'info'
  | 'surface'

export type BaseButtonSize =
  | 'x-small'
  | 'small'
  | 'default'
  | 'large'
  | 'x-large'

export interface BaseButtonProps {
  color?: BaseButtonColor
  variant?: BaseButtonVariant
  size?: BaseButtonSize

  loading?: boolean
  disabled?: boolean

  block?: boolean
  rounded?: boolean | string

  prependIcon?: string
  appendIcon?: string

  elevation?: number | string

  type?: ButtonHTMLAttributes['type']

  ripple?: boolean
}