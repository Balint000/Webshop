import 'vuetify/styles'

import { createVuetify } from 'vuetify'

export default createVuetify({
  theme: {
    defaultTheme: 'light',

    themes:{
      light:{
        colors:{
          primary:'#6366F1',
          secondary:'#8B5CF6',
          success:'#10B981',
          error:'#EF4444'
        }
      }
    }
  },
})