<script setup lang="ts">
import AppHeader from '../components/Navigation/AppHeader.vue'
</script>

<template>
  <v-app>
    <AppHeader />
    <v-main>
      <v-container>
        <router-view />
      </v-container>
    </v-main>
  </v-app>
</template>
