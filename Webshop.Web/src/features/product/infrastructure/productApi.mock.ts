import type { ProductDto } from './types'

export interface ProductListResponse {
  data: ProductDto[]
  totalCount: number
  page: number
  pageSize: number
}

export const productApiMock = {
  async getAll(): Promise<ProductListResponse> {
    await new Promise((r) => setTimeout(r, 800)) // loading szimulálása
    return {
      data: [
        { id: 1, name: 'Fekete póló', description: 'Prémium pamut anyag', price: 4990, stock: 15 },
        {
          id: 2,
          name: 'Farmer nadrág',
          description: 'Slim fit farmernadrág',
          price: 12990,
          stock: 3,
        },
        { id: 3, name: 'Fehér tornacipő', description: 'Könnyű futócipő', price: 18990, stock: 0 },
        { id: 4, name: 'Téli kabát', description: 'Vízálló téli kabát', price: 34990, stock: 8 },
        { id: 5, name: 'Sapka', description: 'Gyapjú téli sapka', price: 2990, stock: 22 },
        { id: 6, name: 'Kesztyű', description: 'Bőr téli kesztyű', price: 5990, stock: 0 },
      ],
      totalCount: 6,
      page: 1,
      pageSize: 12,
    }
  },

  async getById(id: number): Promise<ProductDto> {
    await new Promise((r) => setTimeout(r, 400))
    return { id, name: 'Teszt termék', description: 'Részletes leírás', price: 9990, stock: 5 }
  },
}
