import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { forkJoin } from 'rxjs';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';
import { TableModule } from 'primeng/table';
import { AvatarModule } from 'primeng/avatar';
import { ChartModule } from 'primeng/chart';
import { Aluno } from '../../../domain/entities/aluno.entity';
import { Avaliacao } from '../../../domain/entities/avaliacao.entity';
import { GetAlunosUseCase } from '../../../domain/usecases/aluno/get-alunos.usecase';
import { GetAvaliacoesUseCase } from '../../../domain/usecases/avaliacao/get-avaliacoes.usecase';

interface StatCard {
  label: string;
  value: string;
  icon: string;
  color: string;
}

@Component({
  selector: 'app-dashboard',
  imports: [RouterLink, CardModule, ButtonModule, TagModule, TableModule, AvatarModule, ChartModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss'
})
export class DashboardComponent implements OnInit {
  stats: StatCard[] = [];
  recentAvaliacoes: Avaliacao[] = [];

  chartData = {
    labels: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun'],
    datasets: [{
      label: 'Avaliações realizadas',
      data: [4, 6, 8, 5, 9, 7],
      backgroundColor: 'rgba(99, 102, 241, 0.2)',
      borderColor: 'rgba(99, 102, 241, 1)',
      borderWidth: 2,
      tension: 0.4,
      fill: true
    }]
  };

  chartOptions = {
    plugins: { legend: { display: false } },
    scales: { y: { beginAtZero: true, ticks: { stepSize: 2 } } }
  };

  constructor(
    private readonly getAlunosUseCase: GetAlunosUseCase,
    private readonly getAvaliacoesUseCase: GetAvaliacoesUseCase
  ) {}

  ngOnInit(): void {
    forkJoin({
      alunos: this.getAlunosUseCase.execute(),
      avaliacoes: this.getAvaliacoesUseCase.execute()
    }).subscribe(({ alunos, avaliacoes }) => {
      this.buildStats(alunos, avaliacoes);
      this.recentAvaliacoes = avaliacoes.slice(0, 4);
    });
  }

  private buildStats(alunos: Aluno[], avaliacoes: Avaliacao[]): void {
    const mes = new Date().getMonth() + 1;
    const avaliacoesMes = avaliacoes.filter(a => {
      const [, m] = a.data.split('/');
      return parseInt(m) === mes;
    });

    this.stats = [
      { label: 'Total de Alunos',       value: String(alunos.length),                                         icon: 'pi pi-users',     color: 'blue'   },
      { label: 'Avaliações este mês',   value: String(avaliacoesMes.length),                                  icon: 'pi pi-clipboard', color: 'green'  },
      { label: 'Avaliações pendentes',  value: String(avaliacoes.filter(a => a.status === 'Pendente').length), icon: 'pi pi-clock',     color: 'orange' },
      { label: 'Alunos ativos',         value: String(alunos.filter(a => a.status === 'Ativo').length),        icon: 'pi pi-check',     color: 'purple' },
    ];
  }

  getAvatar(nome: string): string {
    return nome.charAt(0).toUpperCase();
  }

  getSeverity(status: string): 'success' | 'warn' | undefined {
    return status === 'Concluída' ? 'success' : 'warn';
  }
}
