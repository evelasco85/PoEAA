package com.Interfaces;

/**
 * Created by aiko on 4/1/17.
 */

public interface ITransactionScriptConcrete<TInput, TOutput> extends ITransactionScript {
    TInput GetInput();
    void SetInput(TInput input);

    TOutput GetOutput();
    void SetOutput(TOutput output);

    TOutput ExecutionBody();
}
