import { Product } from '../domain/models/Product'
import type { ProductDto } from './types'

export function toProduct(dto: ProductDto): Product {
  return new Product(dto.id, dto.name, dto.description, dto.price, dto.stock)
}

export function toProductList(dtos: ProductDto[]): Product[] {
  return dtos.map(toProduct)
}
