import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgbModal, ModalDismissReasons, NgbModalConfig } from '@ng-bootstrap/ng-bootstrap';

import { IFuncionario } from 'src/app/models/funcionario';
import { Login } from 'src/app/models/login';
import { IUsuarioLogin } from 'src/app/models/usuario-login';
import { FuncionariosService } from 'src/app/services/funcionarios.service';
import { domainToASCII } from 'url';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  closeResult = '';
  resposta = {} as IUsuarioLogin;
  login = {} as IFuncionario;
  desabilitaMsgErroLogin: boolean;
  msgErroLogin: string;
  formNovoUsuario: FormGroup;

  constructor( private modalService: NgbModal,
               private config: NgbModalConfig,
               private funcionariosService: FuncionariosService,
               private router: Router) { 
    config.backdrop = 'static';
    config.keyboard = false;
    this.desabilitaMsgErroLogin = true;
    this.criarFormNovoUsuario({} as Login);
  }

  ngOnInit(): void {
  }

  criarFormNovoUsuario(l: Login) {
    this.formNovoUsuario = new FormGroup({
      usuario: new FormControl(l.usuario, [Validators.required, Validators.minLength(5), Validators.max(100)]),
      senha: new FormControl(l.senha, [Validators.required, Validators.minLength(8), Validators.max(100)]),
      confirmarSenha: new FormControl(l.confirmarSenha, [Validators.required, Validators.minLength(8), Validators.max(100)])
    })
  }

  cadastrar() {
    this.funcionariosService.criarUsuarioLogin(this.formNovoUsuario.value)
                            .subscribe((dados) => {
                              console.error(dados);
                              this.desabilitaMsgErroLogin = false;
                              this.msgErroLogin = `Usuário ${dados["usuario"]} criado com sucesso`;
                              this.modalService.dismissAll();
                            },
                            (erro) => {
                              console.error(erro);
                              this.desabilitaMsgErroLogin = false;
                              this.msgErroLogin = erro.error;
                              this.modalService.dismissAll();
                            });
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
    this.funcionariosService.fazerLogin(this.login).subscribe(
      (dadosRetorno) => {
        this.resposta.dataHora = dadosRetorno["dataHora"];
        this.resposta.token = dadosRetorno["token"];
        this.resposta.funcionarioLogin = dadosRetorno["funcionarioLogin"];
        console.warn(this.resposta);
        sessionStorage.setItem('usuarioLogado', JSON.stringify(dadosRetorno));
        this.funcionariosService.usuarioLogado = this.resposta;
        this.router.navigate(['/home']);
      },
      (erro) => {
        console.error(erro);
        this.desabilitaMsgErroLogin = false;
        this.msgErroLogin = erro.error;
        this.modalService.dismissAll();
      }
    )
  }

}
