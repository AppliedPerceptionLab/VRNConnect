# This is a sample Python script.

import numpy as np
# Press ⌃R to execute it or replace it with your code.
# Press Double ⇧ to search everywhere for classes, files, tool windows, actions, and settings.
import pandas as pd
# importing the data
from numpy import ndarray

df_region_color = pd.read_csv('HCP-MMP1_RegionColor.csv', skipfooter=19, engine='python')
df_region_list = pd.read_csv('HCP-MMP1_UniqueRegionList.csv')
df_main = pd.read_csv('hcpmmp1.csv', skipfooter=19, header=None, engine='python')
ndarray_sub = np.asarray(list(df_main.get(0)))
ndarray_main = np.empty((1, 360))
for x in range(ndarray_sub.size):
    tmp: ndarray = np.resize(np.asarray(dtype=float, a=ndarray_sub[x].split(' ')), (1, 360))
    # print(tmp)
    np.concatenate((ndarray_main, tmp))

# ndarray_main = np.reshape(ndarray_main,(359,359))
# columns = range(0,360)
# df_main = pd.DataFrame(data=df_main, index=columns).T
print(df_main)
# print(df_main.dtypes)
print(df_region_list)
print(df_region_color)
print(ndarray_main)

# clean_main_df = pd.DataFrame()
#
# for row in df_main.iterrows():
#      print(row.__getitem__(1).values.__getitem__(0).split(' '))

# print(clean_df)

print('Result of the algorithms:')
# bct.assortativity_wei(CIJ=ndarray_main,flag=0)
# print('Result of the algorithms:'+ bct.assortativity_wei(CIJ=df_main))


### addind the indices for better understanding
# result_index = df_region_list.columns

# print(result_index)

#result = pd.DataFrame(data=result, index=result_index).T
#result.to_csv('res.csv')

# See PyCharm help at https://www.jetbrains.com/help/pycharm/
