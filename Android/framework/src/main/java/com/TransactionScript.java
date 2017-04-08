package com;

import com.Interfaces.ITransactionScriptConcrete;
import com.Interfaces.IUnitOfWork;
import com.Interfaces.Registries.IMapperRegistry;
import com.Interfaces.Registries.IRepositoryRegistry;

/**
 * Created by aiko on 4/1/17.
 */

public abstract class TransactionScript<TInput, TOutput> implements ITransactionScriptConcrete<TInput, TOutput> {
        TInput _input;

        public TInput GetInput()
        {
        return _input;
        }

        public void SetInput(TInput input)
        {
            _input = input;
        }

    TOutput _output;

    public TOutput GetOutput()
    {
     return _output;
    }

    public void SetOutput(TOutput output)
    {
        _output = output;
    }

    IRepositoryRegistry _repositoryRegistry;
    IMapperRegistry _mapperRegistry;

    public IRepositoryRegistry GetRepositoryRegistry(){return _repositoryRegistry;}
    public IMapperRegistry GetMapperRegistry(){return _mapperRegistry;}

    protected TransactionScript(IRepositoryRegistry repositoryRegistry, IMapperRegistry mapperRegistry)
    {
        _repositoryRegistry = repositoryRegistry;
        _mapperRegistry = mapperRegistry;
    }

    public void PreExecuteBody(){}

    public abstract TOutput ExecutionBody();

    public void PostExecuteBody(){}

    public void RunScript()
    {
        PreExecuteBody();

        _output = ExecutionBody();

        PostExecuteBody();
    }

    public IUnitOfWork CreateUnitOfWork()
    {
        return null;
    }
}
