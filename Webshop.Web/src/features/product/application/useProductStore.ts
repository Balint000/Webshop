import { defineStore } from 'pinia'
import { ref, reactive } from 'vue'
import { productApiMock } from '../infrastructure/productApi.mock' // ← mock
import { toProductList, toProduct } from '../infrastructure/productMapper'
import type { Product } from '../domain/models/Product'

export const useProductStore = defineStore('product', () => {
  const products = ref<Product[]>([])
  const loading = ref(false)
  const error = ref<string | null>(null)
  const totalCount = ref(0)

  const query = reactive({ page: 1, pageSize: 12 })

  async function fetchProducts() {
    loading.value = true
    error.value = null
    try {
      const response = await productApiMock.getAll()
      products.value = toProductList(response.data)
      totalCount.value = response.totalCount
    } catch {
      error.value = 'Nem sikerült betölteni a termékeket.'
    } finally {
      loading.value = false
    }
  }

  function setQuery(params: Partial<typeof query>) {
    Object.assign(query, params)
    query.page = 1
    fetchProducts()
  }

  return { products, loading, error, totalCount, query, fetchProducts, setQuery }
})
