import { ApplicationConfig, provideBrowserGlobalErrorListeners, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { providePrimeNG } from 'primeng/config';
import Aura from '@primeng/themes/aura';

import { routes } from './app.routes';
import { AlunoRepository } from './domain/repositories/aluno.repository';
import { AlunoRepositoryImpl } from './data/repositories/aluno.repository.impl';
import { AvaliacaoRepository } from './domain/repositories/avaliacao.repository';
import { AvaliacaoRepositoryImpl } from './data/repositories/avaliacao.repository.impl';
import { authInterceptor } from './core/interceptors/auth.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideAnimationsAsync(),
    provideHttpClient(withInterceptors([authInterceptor])),
    providePrimeNG({
      theme: {
        preset: Aura,
        options: { darkModeSelector: '.dark-mode' }
      }
    }),
    { provide: AlunoRepository,     useClass: AlunoRepositoryImpl     },
    { provide: AvaliacaoRepository, useClass: AvaliacaoRepositoryImpl },
  ]
};
