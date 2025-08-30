import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import fs from 'fs'
import path from 'path'

// https://vitejs.dev/config/
export default defineConfig({
    plugins: [react()],
    server: {
        host: '0.0.0.0',
        port: Number(process.env.VITE_CLIENT_HTTPS_PORT) || 443,
        https: {
            cert: fs.readFileSync(path.resolve('/SSL/' + process.env.VITE_HOSTNAME_CLIENT + '.crt')),
            key: fs.readFileSync(path.resolve('/SSL/' + process.env.VITE_HOSTNAME_CLIENT + '.key'))
        },
        hmr: {
            protocol: 'wss',
            host: process.env.VITE_HOSTNAME_CLIENT,
            port: Number(process.env.VITE_CLIENT_HTTPS_PORT) || 443
        }
    },
    preview: {
        host: '0.0.0.0',
        port: Number(process.env.VITE_CLIENT_HTTPS_PORT) || 443,
        https: {
            cert: fs.readFileSync(path.resolve('/SSL/' + process.env.VITE_HOSTNAME_CLIENT + '.crt')),
            key: fs.readFileSync(path.resolve('/SSL/' + process.env.VITE_HOSTNAME_CLIENT + '.key'))
        }
    }
})
