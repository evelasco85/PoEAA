package com;

import com.Interfaces.IDataSynchronizationManager;

/**
 * Created by aiko on 3/4/17.
 */

public class DataSynchronizationManager {
    static IDataSynchronizationManager s_instance;

    public static IDataSynchronizationManager GetInstance()
    {
        return s_instance;
    }
}
