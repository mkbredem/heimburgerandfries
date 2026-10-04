// @ts-check
import { defineConfig } from 'astro/config';

export default defineConfig({
  site: 'https://heimburgerandfries.com',
  output: 'static',
  trailingSlash: 'always',
  build: {
    // Keep CSS in files so the Content Security Policy can stay strict.
    inlineStylesheets: 'never',
  },
});
