<script setup lang="ts">
import BaseButton from '@/components/button/BaseButton.vue'

</script>

<template>
  <h1>You did it!</h1>
  <p>
    Visit <a href="https://vuejs.org/" target="_blank" rel="noopener">vuejs.org</a> to read the
    documentation
  </p>
    <v-app>
    <v-main>
      <v-container>

        <BaseButton>
          Alap gomb
        </BaseButton>

        <BaseButton variant="primary">
          Kosárba
        </BaseButton>

        <BaseButton
          variant="gradient"
          size="large"
          >
          Vásárlás
        </BaseButton>

        <BaseButton
          variant="danger"
          >
          Törlés
        </BaseButton>

        <BaseButton
          variant="ghost"
          >
          Mégsem
        </BaseButton>

        <BaseButton
          variant="glass"
          >
          Profil
        </BaseButton>

      </v-container>
    </v-main>
  </v-app>
</template>

<style scoped></style>
