import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal, ModalDismissReasons, NgbModalConfig } from '@ng-bootstrap/ng-bootstrap';

import { IFuncionario } from 'src/app/models/funcionario';
import { IUsuarioLogin } from 'src/app/models/usuario-login';
import { FuncionariosService } from 'src/app/services/funcionarios.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  closeResult = '';
  resposta = {} as IUsuarioLogin;
  login = {} as IFuncionario;

  constructor( private modalService: NgbModal,
               private config: NgbModalConfig,
               private funcionariosService: FuncionariosService,
               private router: Router) { 
    config.backdrop = 'static';
    config.keyboard = false;
  }

  ngOnInit(): void {
  }

  cadastrar() {
    alert('cadastrar');
    this.modalService.dismissAll();
  }

  open(content) {
    console.log(content);
    this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title' }).result.then((result) => {
      this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });
  }

  private getDismissReason(reason: any): string {
    if (reason === ModalDismissReasons.ESC) {
      return 'by pressing ESC';
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return 'by clicking on a backdrop';
    } else {
      return `with: ${reason}`;
    }
  }

  fazerLogin(){
    console.info(this.login);
    this.funcionariosService.fazerLogin(this.login).subscribe(
      (dadosRetorno) => {
        console.log(dadosRetorno);
        this.resposta.dataHora = dadosRetorno["dataHora"];
        this.resposta.token = dadosRetorno["token"];
        this.resposta.funcionarioLogin = dadosRetorno["funcionarioLogin"];
        console.warn(this.resposta);
        sessionStorage.setItem('usuarioLogado', JSON.stringify(dadosRetorno));
        this.router.navigate(['/home']);
      },
      (erro) => {
        console.error(erro);
        console.error(erro.status);
        console.error(erro.error);
        this.modalService.dismissAll();
      }
    )
  }

}
