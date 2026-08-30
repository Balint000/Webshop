import 'vuetify/styles'

import { createVuetify } from 'vuetify'
import * as components from 'vuetify/components'
import * as directives from 'vuetify/directives'

export default createVuetify({
  components,
  directives,

  theme: {
    defaultTheme: 'light',

    themes:{
      light:{
        colors:{
          primary:'#A9FD96',
          secondary:'#8B5CF6',
          success:'#10B981',
          error:'#EF4444'
        }
      }
    }
  },
})
