import api from "@/shared/api/axios.js"
import type { ProductListResponse } from '../interfaces/Products.js'


export async function fetchProducts(page = 1, pageSize = 20): Promise<ProductListResponse> {
  const res = await api.get<ProductListResponse>('/products', {
    params: { Page: page, PageSize: pageSize },
  })
  return res.data
}
