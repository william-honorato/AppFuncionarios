import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal, NgbModalConfig } from '@ng-bootstrap/ng-bootstrap';
import { IFuncionario } from 'src/app/models/funcionario';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

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
  formAdicionar: FormGroup;
  formEditar: FormGroup;

  constructor(private modalService: NgbModal,
    private config: NgbModalConfig,
    private funcionariosService: FuncionariosService,
    private router: Router) {

    config.backdrop = 'static';
    config.keyboard = false;

    this.formAdicionar = this.criarForm({} as IFuncionario);
    this.formEditar = this.criarForm({} as IFuncionario);
  }

  criarForm(f: IFuncionario): FormGroup {
    return new FormGroup({
      nome: new FormControl(f.nome, [Validators.required, Validators.minLength(5), Validators.max(150)]),
      email: new FormControl(f.email, [Validators.email, Validators.max(100)]),
      dataNascimento: new FormControl(f.dataNascimento, [Validators.minLength(10), Validators.max(100)]),
      usuario: new FormControl(f.usuario, [Validators.required, Validators.minLength(5), Validators.max(100)]),
      senha: new FormControl(f.senha, [Validators.required, Validators.minLength(8), Validators.max(100)])
    });
  }

  ngOnInit(): void {
    this.buscarFuncionarios();
  }

  buscarFuncionarios(){
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

        if(erro.status == 401){
          sessionStorage.clear();
          this.router.navigate(['/login']);
        }
      }
    )
  }

  deletar(){
    this.funcionariosService.deletarFuncionario(this.funcionarioModal)
                              .subscribe( data => {
                                this.buscarFuncionarios();
                              },
                              (erro) => {
                                this.buscarFuncionarios();
                              });
  }

  setarFuncionarioModal(f: IFuncionario){
    this.funcionarioModal = f;
  }

  adicionarFuncionario() {
    this.funcionariosService.criarFuncionario(this.formAdicionar.value)
    .subscribe((dados) =>{
      console.log(dados);
      this.buscarFuncionarios();
    },
    (erro) =>{
      console.log(erro);
      this.buscarFuncionarios();
    });
    
    this.formAdicionar.reset({} as IFuncionario);
  }

  editarFuncionario(){
    console.log(this.formEditar.value);
  }

  sairApp(){
    sessionStorage.clear();
    this.router.navigate(['/login']);
  }
}
