using Flow.Application.MessageBrokers;
using Flow.Core.DTOs;
using Flow.Core.Repositories;

namespace Flow.Application.UseCases.Transaction
{
    public class ExecuteTransactionUseCase : IExecuteTransactionUseCase
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUnityOfWork _unityOfWork;
        private readonly KafkaProducer _kafkaProducer;
        public ExecuteTransactionUseCase(
            ITransactionRepository transactionRepository,
            IUnityOfWork unityOfWork,
            KafkaProducer kafkaProducer)
        {
            _transactionRepository = transactionRepository;
            _unityOfWork = unityOfWork;
            _kafkaProducer = kafkaProducer;
        }
        
        public async Task Execute(TransactionDTO request)
        {
            var transaction = new Flow.Core.Entities.Transaction(request.Value, request.Type);
            await _transactionRepository.AddTransaction(transaction);
            await _unityOfWork.Commit();
            await _kafkaProducer.ProduceAsync(transaction);
        }
    }
}