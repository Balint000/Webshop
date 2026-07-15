import { apiClient } from '@/shared/api/apiClient'
import type { ProductDto } from './types'

export interface ProductQuery {
  page?: number
  pageSize?: number
  search?: string
  minPrice?: number
  maxPrice?: number
  sortBy?: string
  descending?: boolean
}

export interface ProductListResponse {
  data: ProductDto[]
  totalCount: number
  page: number
  pageSize: number
}

export const productApi = {
  async getAll(query: ProductQuery = {}): Promise<ProductListResponse> {
    const { data } = await apiClient.get('/products', { params: query })
    return data
  },

  async getById(id: number): Promise<ProductDto> {
    const { data } = await apiClient.get(`/products/${id}`)
    return data
  },
}
