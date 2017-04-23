*The intent of the implementation of Lazy Load is to lessen the load in 'back-end' processing

*This will prevent bulk(up-front) data retrieval and object graph pull

*On-demand data loading has been implemented through list(LazyLoadList<...>) element access

*Lazy loading is applicable to only one business-transaction span, thus it is of less help in maintaining state for UI/View purposes

*The loading checks is performed through list per-element access as opposed to traditional per-element-per-field

*Linq manipulation is not accounted since lazy loading is implemented primitively (applicable in Android/Java development)