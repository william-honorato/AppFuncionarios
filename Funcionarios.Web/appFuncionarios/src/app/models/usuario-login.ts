import { IFuncionario } from './funcionario';

export interface IUsuarioLogin {
    dataHora: Date;
    token: string;
    funcionarioLogin: IFuncionario;
}
