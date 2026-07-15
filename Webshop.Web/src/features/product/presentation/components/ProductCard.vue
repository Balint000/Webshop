<script setup lang="ts">
import type { Product } from '../../domain/models/Product'
defineProps<{ product: Product }>()
</script>

<template>
  <v-card :to="`/products/${product.id}`" hover rounded="lg" class="h-100">
    <v-card-title>{{ product.name }}</v-card-title>
    <v-card-subtitle>{{ product.formattedPrice }}</v-card-subtitle>
    <v-card-text class="text-truncate">{{ product.description }}</v-card-text>
    <v-card-actions>
      <v-chip :color="product.isInStock ? 'success' : 'error'" size="small">
        {{ product.isInStock ? `${product.stock} db raktáron` : 'Elfogyott' }}
      </v-chip>
    </v-card-actions>
  </v-card>
</template>
