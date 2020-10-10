import { IFuncionario } from './funcionario';

export interface IRespostaLogin {
    dataHora: Date;
    token: string;
    funcionarioLogin: IFuncionario;
}
