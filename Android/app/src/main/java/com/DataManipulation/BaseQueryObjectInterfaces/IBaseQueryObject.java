package com.DataManipulation.BaseQueryObjectInterfaces;

/**
 * Created by aiko on 3/4/17.
 */

public interface IBaseQueryObject {
    Class GetSearchInputType();
    Object GetSearchInputObject();
    void SetSearchInputObject(Object inputObject);
}