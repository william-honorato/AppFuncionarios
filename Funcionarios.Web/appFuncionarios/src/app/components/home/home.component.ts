import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal, NgbModalConfig } from '@ng-bootstrap/ng-bootstrap';
import { IFuncionario } from 'src/app/models/funcionario';

import { IUsuarioLogin } from 'src/app/models/usuario-login';
import { FuncionariosService } from 'src/app/services/funcionarios.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})

export class HomeComponent implements OnInit {

  closeResult = '';
  funcionarios = [] as IFuncionario[];
  funcionarioModal = {} as IFuncionario;

  constructor(private modalService: NgbModal,
    private config: NgbModalConfig,
    private funcionariosService: FuncionariosService,
    private router: Router) {

    config.backdrop = 'static';
    config.keyboard = false;
  }

  ngOnInit(): void {
    this.funcionariosService.buscarFuncionarios().subscribe(
      (dadosRetorno) => {
        this.funcionarios = <IFuncionario[]>dadosRetorno;
        this.modalService.dismissAll();
      },
      (erro) => {
        console.error(erro);
        console.error(erro.status);
        console.error(erro.error);
        this.modalService.dismissAll();
      }
    )

    console.log("onInit Home");
  }

  deletar(){
    this.funcionariosService.deletarFuncionario(this.funcionarioModal);
    const index = this.funcionarios.indexOf(this.funcionarioModal);

    console.log("index", index);
    if(index != -1) {
      this.funcionarios.slice(index, 1);
      console.log("lenght", this.funcionarios.length);
    }

    this.modalService.dismissAll();
  }

  setarFuncionarioModal(f: IFuncionario){
    this.funcionarioModal = f;
  }

}
