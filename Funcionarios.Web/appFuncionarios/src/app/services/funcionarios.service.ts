import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { IFuncionario } from "../models/funcionario"
import { IUsuarioLogin } from '../models/usuario-login';

@Injectable({
  providedIn: 'root'
})
export class FuncionariosService {

  private URL_BASE = "https://localhost:44310/api";
  usuarioLogado = {} as IUsuarioLogin;

  // Headers
  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  }
  constructor(private http: HttpClient) { 
    const jsonUsuario = JSON.parse(sessionStorage.getItem("usuarioLogado"));
    this.usuarioLogado.dataHora = new Date(jsonUsuario["dataHora"]);
    this.usuarioLogado.token = jsonUsuario["token"];
    this.usuarioLogado.funcionarioLogin = jsonUsuario["funcionarioLogin"];

    console.error(this.usuarioLogado);
  }

  public fazerLogin(func: IFuncionario){
    return this.http.post<IFuncionario>(`${this.URL_BASE}/funcionarios/login`, JSON.stringify(func), this.httpOptions);
  }

  public criarUsuario(id: Number){
    return this.http.get(`${this.URL_BASE}/funcionarios/${id}`);
  }

  //Metodos abaixo precisam estar autenticados
  public buscarFuncionario(id: Number){
    return this.http.get(`${this.URL_BASE}/funcionarios/${id}`);
  }

  public buscarFuncionarios(){

    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json',
        Authorization: `Bearer ${this.usuarioLogado.token}`
      })
    };

    return this.http.get<IFuncionario[]>(`${this.URL_BASE}/funcionarios`, httpOptions);
  }

  public criarFuncionario(funcionario: IFuncionario){
    return this.http.get(`${this.URL_BASE}/funcionarios`);
  }

  public atualizarFuncionario(funcionario: IFuncionario){
    return this.http.get(`${this.URL_BASE}/funcionarios`);
  }

  public deletarFuncionario(funcionario: IFuncionario){

    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json',
        Authorization: `Bearer ${this.usuarioLogado.token}`
      })
    };

    const urlAPI = `${this.URL_BASE}/funcionarios/${funcionario.id}`;
    console.log("URL:", urlAPI);
    return this.http.delete(urlAPI, httpOptions);
  }
}
