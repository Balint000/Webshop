<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useProductStore } from '../../application/useProductStore'
import ProductCard from '../components/ProductCard.vue'

const store = useProductStore()
const searchText = ref('')
const sortBy = ref('name')

const sortOptions = [
  { title: 'Név szerint', value: 'name' },
  { title: 'Ár szerint (növekvő)', value: 'price' },
  { title: 'Ár szerint (csökkenő)', value: 'price_desc' },
]

const totalPages = computed(() => Math.ceil(store.totalCount / store.query.pageSize!))

onMounted(() => store.fetchProducts())
</script>

<template>
  <v-container>
    <!-- Loading skeleton -->
    <v-row v-if="store.loading">
      <v-col v-for="n in 6" :key="n" cols="12" sm="6" md="4" lg="3">
        <v-skeleton-loader type="card" />
      </v-col>
    </v-row>

    <!-- Hiba -->
    <v-alert v-else-if="store.error" type="error">
      {{ store.error }}
    </v-alert>

    <!-- Termék grid -->
    <v-row v-else>
      <v-col v-for="product in store.products" :key="product.id" cols="12" sm="6" md="4" lg="3">
        <ProductCard :product="product" />
      </v-col>
    </v-row>
  </v-container>
</template>
