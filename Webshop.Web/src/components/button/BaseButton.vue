<script setup lang="ts">
import { computed, useSlots } from 'vue'
import { VBtn } from 'vuetify/components'

import type { BaseButtonProps } from './BaseButton.types'

import './BaseButton.scss'

const props = withDefaults(defineProps<BaseButtonProps>(), {
  color: 'primary',
  size: 'default',

  loading: false,
  disabled: false,

  block: false,
  rounded: false,

  ripple: true,

  type: 'button'
})

const emit = defineEmits<{
  (e: 'click', event: MouseEvent): void
}>()

const slots = useSlots()

const isIconOnly = computed(() => {
  return (
    !slots.default &&
    (!!props.prependIcon || !!props.appendIcon)
  )
})

const classes = computed(() => [
  'base-button',

  `base-button--${props.variant}`,

  {
    'base-button--block': props.block,
    'base-button--loading': props.loading,
    'base-button--rounded': props.rounded,
    'base-button--icon-only': isIconOnly.value,

    'base-button--small': props.size === 'small',
    'base-button--large': props.size === 'large'
  }
])

function onClick(event: MouseEvent) {
  if (props.loading || props.disabled)
    return

  emit('click', event)
}
</script>

<template>
    <VBtn
    :class="classes"
    :color="undefined"
    variant="plain"
    >
    <slot />
  </VBtn>
</template>