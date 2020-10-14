import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal, NgbModalConfig } from '@ng-bootstrap/ng-bootstrap';
import { IFuncionario } from 'src/app/models/funcionario';
import { FormControl, FormGroup, Validators } from '@angular/forms';

import { FuncionariosService } from 'src/app/services/funcionarios.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})

export class HomeComponent implements OnInit {

  @ViewChild('btnFecharModalEditar') btnFecharModalEditar;
  @ViewChild('btnFecharModalDeletar') btnFecharModalDeletar;
  @ViewChild('btnFecharModalAdicionar') btnFecharModalAdicionar;

  closeResult = '';
  funcionarios = [] as IFuncionario[];
  funcionarioModal = {} as IFuncionario;
  formAdicionar: FormGroup;
  formEditar: FormGroup;
  BirthDate: Date;

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
      id: new FormControl(f.id),
      nome: new FormControl(f.nome, [Validators.required, Validators.minLength(5), Validators.max(150)]),
      email: new FormControl(f.email, [Validators.email, Validators.max(100)]),
      dataNascimento: new FormControl(f.dataNascimento),
      usuario: new FormControl(f.usuario, [Validators.required, Validators.minLength(5), Validators.max(50)]),
      senha: new FormControl(f.senha, [Validators.required, Validators.minLength(8), Validators.max(50)])
    });
  }

  ngOnInit(): void {
    this.buscarFuncionarios();
    this.BirthDate = new Date();
  }

  public fecharModalEditar() {
    this.btnFecharModalEditar.nativeElement.click();
  }

  public fecharModalDeletar() {
    this.btnFecharModalDeletar.nativeElement.click();
  }

  public fecharModalAdicionar() {
    this.btnFecharModalAdicionar.nativeElement.click();
  }

  buscarFuncionarios(){
    this.funcionariosService.buscarFuncionarios().subscribe(
      (dadosRetorno) => {
        this.funcionarios = <IFuncionario[]>dadosRetorno;
        this.modalService.dismissAll();
      },
      (erro) => {
        console.error(erro);
        this.modalService.dismissAll();

        const session = sessionStorage.getItem("usuarioLogado");
        if(erro.status == 401 || session == null){
          sessionStorage.clear();
          this.router.navigate(['/login']);
        }
      }
    )
  }

  deletar(){
    this.funcionariosService.deletarFuncionario(this.funcionarioModal)
                              .subscribe( data => {
                                this.fecharModalDeletar();
                                this.buscarFuncionarios();
                              },
                              (erro) => {
                                this.fecharModalDeletar();
                                this.buscarFuncionarios();
                              });
  }

  setarFuncionarioModal(f: IFuncionario){
    this.funcionarioModal = f;
  }

  adicionarFuncionario() {
    this.formAdicionar.get('id').setValue(0); //API não aceita nulo
    this.funcionariosService.criarFuncionario(this.formAdicionar.value)
                            .subscribe((dados) =>{
                              console.log(dados);
                              this.fecharModalAdicionar();
                              this.buscarFuncionarios();
                            },
                            (erro) =>{
                              console.log(erro);
                              this.fecharModalAdicionar();
                              this.buscarFuncionarios();
                            });
    
    this.formAdicionar.reset({} as IFuncionario);
  }

  editarFuncionario(f: IFuncionario){
    this.formEditar = this.criarForm(f);
  }

  atualizarFuncionario(){
    this.funcionariosService.atualizarFuncionario(this.formEditar.value)
                            .subscribe((dados) =>{
                              console.log(dados);
                              this.fecharModalEditar();
                              this.buscarFuncionarios();
                            },
                            (erro) =>{
                              console.log(erro);
                              this.fecharModalEditar();
                              this.buscarFuncionarios();
                            });
  }

  sairApp(){
    sessionStorage.clear();
    this.router.navigate(['/login']);
  }
}
