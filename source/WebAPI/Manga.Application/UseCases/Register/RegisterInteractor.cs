﻿namespace Manga.Application.UseCases.Register
{
    using System.Threading.Tasks;
    using Manga.Domain.Customers;
    using Manga.Domain.ValueObjects;
    using Manga.Application.Repositories;
    using Manga.Domain.Accounts;
    using Manga.Application.Outputs;

    public class RegisterInteractor : IInputBoundary<RegisterInput>
    {
        private readonly ICustomerWriteOnlyRepository customerWriteOnlyRepository;
        private readonly IAccountWriteOnlyRepository accountWriteOnlyRepository;
        private readonly IOutputBoundary<RegisterOutput> outputBoundary;
        private readonly IOutputConverter responseConverter;
        
        public RegisterInteractor(
            ICustomerWriteOnlyRepository customerWriteOnlyRepository,
            IAccountWriteOnlyRepository accountWriteOnlyRepository,
            IOutputBoundary<RegisterOutput> outputBoundary,
            IOutputConverter responseConverter)
        {
            this.customerWriteOnlyRepository = customerWriteOnlyRepository;
            this.accountWriteOnlyRepository = accountWriteOnlyRepository;
            this.outputBoundary = outputBoundary;
            this.responseConverter = responseConverter;
        }

        public async Task Process(RegisterInput message)
        {
            Customer customer = new Customer(
                new PIN(message.PIN),
                new Name(message.Name));

            Account account = new Account();
            Credit credit = new Credit(new Amount(message.InitialAmount));
            account.Deposit(credit);

            customer.Register(account.Id);

            await customerWriteOnlyRepository.Add(customer);
            await accountWriteOnlyRepository.Add(account);

            CustomerOutput customerOutput = responseConverter.Map<CustomerOutput>(customer);
            AccountOutput accountOutput = responseConverter.Map<AccountOutput>(account);
            RegisterOutput output = new RegisterOutput(customerOutput, accountOutput);

            outputBoundary.Populate(output);
        }
    }
}
