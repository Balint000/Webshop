import { createApp } from 'vue'
import { createPinia } from 'pinia'
import vuetify from './plugins/vuetify'
import App from './app/App.vue'
import './main.scss'

const app = createApp(App)

app.use(createPinia())

app.use(vuetify)

app.mount('#app')
