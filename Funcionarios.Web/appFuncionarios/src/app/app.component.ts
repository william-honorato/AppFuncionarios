import { Component, OnInit } from '@angular/core';
import { FuncionariosService } from './services/funcionarios.service';
import { IFuncionario } from './models/funcionario';
import { IRespostaLogin } from './models/resposta-login';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'appFuncionarios';
  func = {} as IFuncionario;
  resp = {} as IRespostaLogin;
  
  constructor(
    private funcionariosService: FuncionariosService
  ){}

  ngOnInit(){
    this.func.usuario = "admin5";
    this.func.senha = "admin123";
  this.funcionariosService.fazerLogin(this.func).subscribe(
    (dadosRetorno) => {
      console.log(dadosRetorno);
      this.resp.dataHora = dadosRetorno["dataHora"];
      this.resp.token = dadosRetorno["token"];
      this.resp.funcionarioLogin = dadosRetorno["funcionarioLogin"];
      console.warn(this.resp);
    },
    (erro) => {
      console.error(erro);
      console.error(erro.status);
      console.error(erro.error);
    }
  )}
}
