import { Component, OnInit } from '@angular/core';
import { FuncionariosService } from './services/funcionarios.service';
import { IFuncionario } from './models/funcionario';
import { IUsuarioLogin } from './models/usuario-login';
import { NgbPaginationModule, NgbAlertModule } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent {
  title = 'appFuncionarios';
  func = {} as IFuncionario;
  resp = {} as IUsuarioLogin;
  
  constructor(
    private funcionariosService: FuncionariosService
  ){}

  ngOnInit() {}
}
