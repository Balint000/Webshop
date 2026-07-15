// src/shared/api/apiClient.ts
import axios from 'axios'

export const apiClient = axios.create({
  baseURL: '/api', // a Vite proxy továbbítja a backendnek
  headers: { 'Content-Type': 'application/json' },
})
