import { ref } from 'vue'
import { fetchProducts  } from "@/shared/api/composables/useProductsQuery"
import { mockProducts } from "@/shared/api/mocks/mockProducts"
import type { Product } from "@/shared/api/interfaces/Products"

export function useProducts() {
  const products = ref<Product[]>([])
  const loading = ref(true)
  const error = ref<string | null>(null)
  const usingMock = ref(false)

  async function loadProducts() {
    try {
      const result = await fetchProducts(1, 20)
      products.value = result.data
    } catch (err) {
      console.error(err)
      error.value = 'Nem sikerült betölteni a termékeket a szerverről — mock adatok megjelenítve.'
      products.value = mockProducts
      usingMock.value = true
    } finally {
      loading.value = false
    }
  }

  return {
    loadProducts,
    loading,
    error,
    usingMock,
    products
  }
}
