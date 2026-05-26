import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import path from 'path'

// Serve ../publish/ as static root so fetch('/BackTestData/...') resolves
// to publish/BackTestData/ without CORS issues during development.
export default defineConfig({
  plugins: [react()],
  publicDir: path.resolve(__dirname, '../publish'),
})
