import { Component, inject } from '@angular/core';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { LoadingService } from '../../../core/services/loading.service';

@Component({
  selector: 'app-spinner',
  imports: [ProgressSpinnerModule],
  templateUrl: './spinner.html',
  styleUrl: './spinner.scss'
})
export class SpinnerComponent {
  readonly loadingService = inject(LoadingService);
}
