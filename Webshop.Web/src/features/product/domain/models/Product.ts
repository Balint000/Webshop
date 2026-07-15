export class Product {
  constructor(
    public readonly id: number,
    public readonly name: string,
    public readonly description: string,
    public readonly price: number,
    public readonly stock: number,
  ) {}

  get isInStock(): boolean {
    return this.stock > 0
  }

  get formattedPrice(): string {
    return new Intl.NumberFormat('hu-HU', {
      style: 'currency',
      currency: 'HUF',
      maximumFractionDigits: 0,
    }).format(this.price)
  }
}
