import type { RouteRecordRaw } from 'vue-router'
import ProductListPage from "../../components/Products/ProductListPage.vue"

export const routes: RouteRecordRaw[] = [
  {
    path: '/',
    name: 'products',
    component: ProductListPage,
  },
]
