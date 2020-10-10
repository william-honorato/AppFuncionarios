import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { IFuncionario } from "../models/funcionario"

@Injectable({
  providedIn: 'root'
})
export class FuncionariosService {
  // Headers
  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  }
  constructor(private http: HttpClient) { }

  public fazerLogin(func: IFuncionario){
    return this.http.post<IFuncionario>("https://localhost:44310/api/funcionarios/login", JSON.stringify(func), this.httpOptions);
  }

  public buscarDados(){
    return this.http.get("https://localhost:44310/weatherforecast");
  }
}
