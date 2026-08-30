export interface Product {
  id: number
  name: string
  price: number
}

export interface ProductListResponse {
  data: Product[]
  totalCount: number
  page: number
  pageSize: number
}
