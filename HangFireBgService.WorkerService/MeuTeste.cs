namespace HangFireBgService.WorkerService
{
    internal interface IMeuTeste
    {
        Task ExecuteAsync(CancellationToken cancellationToken);
    }

    public class MeuTeste(ILogger<MeuTeste> logger) : IMeuTeste
    {
        private readonly ILogger<MeuTeste> _logger = logger;

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Iniciando execução de tarefa.");

            await Task.Delay(2000, cancellationToken);

            _logger.LogInformation("Tarefa executada.");
        }
    }
}
