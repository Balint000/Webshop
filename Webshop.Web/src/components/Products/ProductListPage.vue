<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useProducts } from './composables/useProducts'

const { loadProducts, loading, error, usingMock, products } = useProducts()

onMounted(loadProducts)
</script>

<template>
  <h1 class="text-h4 mb-4">Termékek</h1>

  <v-progress-circular v-if="loading" indeterminate color="primary" />

  <template v-else>
    <v-alert v-if="usingMock" type="warning" class="mb-4">{{ error }}</v-alert>

    <v-row>
      <v-col v-for="p in products" :key="p.id" cols="12" sm="6" md="4">
        <v-card>
          <v-card-title>{{ p.name }}</v-card-title>
          <v-card-subtitle>{{ p.price }} Ft</v-card-subtitle>
        </v-card>
      </v-col>
    </v-row>
  </template>
</template>
