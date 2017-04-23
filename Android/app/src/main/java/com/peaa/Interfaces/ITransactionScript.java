package com.peaa.Interfaces;

/**
 * Created by aiko on 4/1/17.
 */

public interface ITransactionScript extends ITransactionScriptExecution{
    IUnitOfWork CreateUnitOfWork();
    void PreExecuteBody();
    void PostExecuteBody();
}
