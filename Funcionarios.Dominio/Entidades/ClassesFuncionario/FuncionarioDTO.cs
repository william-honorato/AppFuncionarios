namespace Funcionarios.Dominio.Entidades.ClassesFuncionario
{
    public class FuncionarioDTO
    {
        public int ID { get; set; }

        public string Usuario { get; set; }

        public string Senha { get; set; }

        public string Nome { get; set; }

        public string DataNascimento { get; set; }

        public string Email { get; set; }

        public Funcionario CriarFuncionario()
        {
            var funcionario = new Funcionario(Usuario, Senha, Nome, DataNascimento, Email);
            funcionario.ID = ID;
            return funcionario;
        }
    }
}
