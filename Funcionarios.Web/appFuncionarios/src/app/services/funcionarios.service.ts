import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { IFuncionario } from "../models/funcionario"
import { IUsuarioLogin } from '../models/usuario-login';

@Injectable({
  providedIn: 'root'
})
export class FuncionariosService {

  private URL_BASE = "https://localhost:44310/api";
  public usuarioLogado = {} as IUsuarioLogin;

  constructor(private http: HttpClient) { 
    const jsonString = sessionStorage.getItem("usuarioLogado");

    if(jsonString != null){
      const jsonUsuario = JSON.parse(jsonString);
      this.usuarioLogado.dataHora = new Date(jsonUsuario["dataHora"]);
      this.usuarioLogado.token = jsonUsuario["token"];
      this.usuarioLogado.funcionarioLogin = jsonUsuario["funcionarioLogin"];
    }
  }

  pegarHeader(){
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json',
        Authorization: `Bearer ${this.usuarioLogado.token}`
      })
    };

    return httpOptions;
  }

  public fazerLogin(funcionario: IFuncionario){
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json'
      })
    };
    return this.http.post<IFuncionario>(`${this.URL_BASE}/funcionarios/login`, JSON.stringify(funcionario), httpOptions);
  }

  public criarUsuarioLogin(funcionario: IFuncionario){
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json'
      })
    };
    return this.http.post<IFuncionario>(`${this.URL_BASE}/funcionarios/criar`, JSON.stringify(funcionario), httpOptions);
  }

  //Os metodos abaixo precisam estar autenticados
  public buscarFuncionario(id: Number){
    return this.http.get(`${this.URL_BASE}/funcionarios/${id}`);
  }

  public buscarFuncionarios(){
    const httpOptions = this.pegarHeader();
    return this.http.get<IFuncionario[]>(`${this.URL_BASE}/funcionarios`, httpOptions);
  }

  public criarFuncionario(funcionario: IFuncionario){
    const httpOptions = this.pegarHeader();
    return this.http.post<IFuncionario>(`${this.URL_BASE}/funcionarios`, JSON.stringify(funcionario), httpOptions);
  }

  public atualizarFuncionario(funcionario: IFuncionario){
    const httpOptions = this.pegarHeader();
    return this.http.put<IFuncionario>(`${this.URL_BASE}/funcionarios/${funcionario.id}`, JSON.stringify(funcionario), httpOptions);
  }

  public deletarFuncionario(funcionario: IFuncionario){
    const httpOptions = this.pegarHeader();
    return this.http.delete(`${this.URL_BASE}/funcionarios/${funcionario.id}`, httpOptions);
  }
}
